using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CRMApp.Services
{
    public interface INoteService
    {
        List<Note> GetAllNotes();

        List<Note> GetNoteByCustomerId(int customerId);
        List<Note> GetNoteByCustomerId(int customerId, int pageIndex);
        List<Note> GetNotes(NoteFilter filter, string input, int customerId);
        List<Note> GetNotes(NoteFilter filter, string input, int customerId, int pageIndex);

        bool SaveNote(Note note);

        bool DeleteNote(int noteId);
        Note GetNoteById(int noteId);
    }
    public class NoteService : INoteService
    {
        private readonly ApplicationUserIdentityContext _context;
        private readonly IContactService _contactService;
        private readonly IActivityLogger _activityLogger;
        private readonly int PageSize = 5;

        public NoteService(ApplicationUserIdentityContext context, IContactService contactService, IActivityLogger activityLogger)
        {
            _context = context;
            _contactService = contactService;
            _activityLogger = activityLogger;
        }

        public bool DeleteNote(int noteId)
        {
            var currentUser = _contactService.GetCurrentUserAsync().Result;
            var note = GetNoteById(noteId);
            if (note == null)
            {
                return false;
            }
            _context.Notes.Remove(note);
            _activityLogger.LogAsync(Module.Note, currentUser.Id, $"Deleted By {currentUser.NormalizedUserName}", true);
            _context.SaveChanges();
            return true;
        }

        public List<Note> GetAllNotes()
        {
            return _context.Notes.ToList();
        }

        public List<Note> GetNoteByCustomerId(int customerId)
        {
            return _context.Notes.Where(c => c.CustomerId == customerId).ToList();
        }

        public List<Note> GetNoteByCustomerId(int customerId, int pageIndex)
        {
            return _context.Notes.Where(c => c.CustomerId == customerId)
                .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
        }

        public Note GetNoteById(int noteId)
        {
            return _context.Notes.Find(noteId);
        }

        public List<Note> GetNotes(NoteFilter filter, string input, int customerId)
        {
            switch (filter)
            {
                case NoteFilter.Description:

                case NoteFilter.All:
                default:
                    return _context.Notes.Where(c => (!string.IsNullOrWhiteSpace(input)) && (c.CustomerId == customerId) && (c.Description.Contains(input))).ToList();

            }
        }

        public List<Note> GetNotes(NoteFilter filter, string input, int customerId, int pageIndex)
        {
            switch (filter)
            {
                case NoteFilter.Description:

                case NoteFilter.All:
                default:
                    return _context.Notes.Where(c => (!string.IsNullOrWhiteSpace(input)) && (c.CustomerId == customerId) && (c.Description.Contains(input)))
                        .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();

            }
        }

		public bool SaveNote(Note note)
		{
			var currentUser = _contactService.GetCurrentUserAsync().Result;

			if (note.Id == 0)
			{
				note.CreatedAt = DateTime.Now;
				note.CreatedBy = currentUser.Id;

				var customerIdParam = new SqlParameter("@CustomerId", note.CustomerId);
				var descriptionParam = new SqlParameter("@Description", note.Description ?? (object)DBNull.Value);
				var createdByParam = new SqlParameter("@CreatedBy", note.CreatedBy ?? (object)DBNull.Value);
				var createdAtParam = new SqlParameter("@CreatedAt", note.CreatedAt);
				var newIdParam = new SqlParameter("@NewId", SqlDbType.Int)
				{
					Direction = ParameterDirection.Output
				};

				_context.Database.ExecuteSqlRaw(
					"EXEC sp_InsertNote @CustomerId, @Description, @CreatedBy, @CreatedAt, @NewId OUTPUT",
					customerIdParam, descriptionParam, createdByParam, createdAtParam, newIdParam
				);

				note.Id = (int)newIdParam.Value;

				_activityLogger.LogAsync(Module.Note, currentUser.Id, $"Added by {currentUser.NormalizedUserName}", false);
			}
			else
			{
				_context.Notes.Update(note); // Optional: can be replaced with an UpdateNote stored procedure
				_activityLogger.LogAsync(Module.Note, currentUser.Id, $"Updated by {currentUser.NormalizedUserName}", false);
				_context.SaveChanges();
			}

			return true;
		}
	}
}
