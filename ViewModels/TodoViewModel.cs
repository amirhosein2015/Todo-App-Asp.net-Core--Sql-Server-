using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.ViewModels
{
    public class TodoViewModel
    {
        // فیلدهای جستجو و فیلتر
        public string SearchQuery { get; set; } // برای نگه داشتن متن جستجو
        public string PriorityFilter { get; set; } // فیلتر بر اساس اولویت
        public bool? IsFinishedFilter { get; set; } // فیلتر بر اساس وضعیت انجام‌شده (true, false, یا null برای همه)
        public DateTime? StartDateFilter { get; set; } // فیلتر بر اساس تاریخ شروع
        public DateTime? EndDateFilter { get; set; } // فیلتر بر اساس تاریخ پایان

        // لیست Todos برای نمایش
        public List<TodoItemViewModel> Todos { get; set; } = new List<TodoItemViewModel>();
    }

    // مدل برای هر تسک
    public class TodoItemViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required]
        [RegularExpression("(Low|Normal|High)")]
        public string Priority { get; set; } = "Normal"; // Default value
        public DateTime? CreatedAt { get; set; } // تاریخ می‌تونه نال باشه
        public bool IsFinished { get; set; } = false;
        public bool IsArchived { get; set; } = false;
    }
}
