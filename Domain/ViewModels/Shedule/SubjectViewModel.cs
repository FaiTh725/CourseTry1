using CourseTry1.Domain.Entity;

namespace CourseTry1.Domain.ViewModels.Shedule
{
    public class SubjectViewModel
    {
        public string Subject { get; set; } = string.Empty;

        public string Teacher { get; set; } = string.Empty;

        public string Corpus { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string Time {  get; set; } = string.Empty;

        public SubjectViewModel(Subject subject)
        {
            Time = subject.Time;

            string[] parseString = new string[2];
            string[] parseAdvance = null;

            if (subject.Name.Contains('|'))
            {
                parseString = subject.Name.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                parseAdvance = parseString[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);


                if (parseString[0] == "Физическая культура")
                {
                    Subject = parseString[0];
                    Teacher = parseString[1];
                }
                else
                {
                    Audience = parseAdvance[^2];
                    Corpus = parseAdvance[^1][2..];
                    Teacher = parseAdvance[0] + " " + parseAdvance[1];
                    Subject = parseString[0];
                }
            }
            else
            {
                var split = subject.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                Audience = split[^2];
                Corpus = split[^1][2..];

                for (int i = 0; i < split.Length - 2; i++)
                {
                    Subject += split[i] + " ";
                }
            }
        }
    }
}
