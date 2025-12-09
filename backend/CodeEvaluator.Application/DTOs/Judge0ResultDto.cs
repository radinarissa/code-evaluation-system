using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Application.DTOs
{
    public class Judge0ResultDto
    {
        public string Stdout { get; set; }
        public string Stderr { get; set; }
        public string CompileOutput { get; set; }
        public Judge0Status Status { get; set; }
        public float Time { get; set; }
        public long? Memory { get; set; }
    }

    public class Judge0Status
    {
        public string Description { get; set; }
    }

}
