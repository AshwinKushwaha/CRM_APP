using CRMApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Controllers
{
	public class NoteController : Controller
	{
		private readonly INoteService _noteService;

		public NoteController(INoteService noteService)
        {
			_noteService = noteService;
		}
        public IActionResult Index()
		{
			var notes = _noteService.GetAllNotes();
			return View(notes);
		}

		
	}
}
