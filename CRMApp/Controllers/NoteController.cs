using CRMApp.Services;
using CRMApp.ViewModels;
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

		[HttpPost]
		public IActionResult SaveNote(CustomerViewModel viewModel)
		{
			if(viewModel.Note != null)
			{
				_noteService.SaveNote(viewModel.Note);
				return RedirectToAction("Details", "Customer", new { Id = viewModel.Note.CustomerId });
			}

			return View(viewModel);
		}


		[HttpPost]
		public IActionResult DeleteNote(int id)
		{
			var deletedNote = _noteService.GetNoteById(id);
			_noteService.DeleteNote(id);
			return RedirectToAction("Details", "Customer", new { Id = deletedNote.CustomerId });
		}

		
	}
}
