using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Common
{
    public static class EntityValidationConstants
    {

        public static class NoteValidadtion
        {
            public const int TitleMaxLenght = 30;
            public const int TitleMinLenght = 3;

            public const int ContentMaxLenght = 1000;
            public const int ContentMinLenght = 5; 
        }
    }
}
