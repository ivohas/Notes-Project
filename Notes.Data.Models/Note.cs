﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Notes.Common.EntityValidationConstants.NoteValidadtion;

namespace Notes.Data.Models
{
    public class Note
    {
        public Note()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [StringLength(TitleMaxLenght, MinimumLength = TitleMinLenght)]
        public string Title { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public ApplicationUser Author { get; set; }

        [StringLength(ContentMaxLenght, MinimumLength = ContentMinLenght)]
        public string Content { get; set; }

        public bool IsPinned { get; set; }

    }
}
