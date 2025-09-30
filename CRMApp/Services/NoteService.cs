using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CRMApp.Services
{
	public interface INoteService
	{
		List<Note> GetAllNotes();

		List<Note> GetNoteByCustomerId(int customerId);

		bool SaveNote(Note note);

		bool DeleteNote(int noteId);
		Note GetNoteById(int noteId);
	}
	public class NoteService : INoteService
	{
		private readonly ApplicationUserIdentityContext _context;
		private readonly IContactService _contactService;
		private readonly IActivityLogger _activityLogger;

		public NoteService(ApplicationUserIdentityContext context,IContactService contactService,IActivityLogger activityLogger)
        {
			_context = context;
			_contactService = contactService;
			_activityLogger = activityLogger;
		}

		public bool DeleteNote(int noteId)
		{
			var currentUser = _contactService.GetCurrentUserAsync().Result;
			var note = GetNoteById(noteId);
			if (note == null) {
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

		public Note GetNoteById(int noteId)
		{
			return _context.Notes.Find(noteId);
		}

		public bool SaveNote(Note note)
		{
			var currentUser = _contactService.GetCurrentUserAsync().Result;
			if (note.Id == 0)
			{
				note.CreatedAt = DateTime.Now;
				note.CreatedBy = currentUser.Id;
				_context.Notes.Add(note);
				_activityLogger.LogAsync(Module.Note, currentUser.Id, $"Added by {currentUser.NormalizedUserName}", false);
			}
			else
			{
				_context.Notes.Update(note);
				_activityLogger.LogAsync(Module.Note, currentUser.Id, $"Updated by {currentUser.NormalizedUserName}", false);
			}
			_context.SaveChanges();
			return true;
		}
	}
}
