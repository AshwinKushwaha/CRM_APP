using CRMApp.Areas.Identity.Data;
using CRMApp.Models;

namespace CRMApp.Services
{
	public interface INoteService
	{
		List<Note> GetAllNotes();

		List<Note> GetNoteByCustomerId(int customerId);

		bool SaveNote(Note note);
	}
	public class NoteService : INoteService
	{
		private readonly ApplicationUserIdentityContext _context;

		public NoteService(ApplicationUserIdentityContext context)
        {
			_context = context;
		}
        public List<Note> GetAllNotes()
		{
			return _context.Notes.ToList();
		}

		public List<Note> GetNoteByCustomerId(int customerId)
		{
			return _context.Notes.Where(c => c.CustomerId == customerId).ToList();
		}

		public bool SaveNote(Note note)
		{
			if(note.Id == 0)
			{
				_context.Notes.Add(note);
			}
			else
			{
				_context.Notes.Update(note);
			}
			_context.SaveChanges();
			return true;
		}
	}
}
