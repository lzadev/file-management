namespace fileManagement.Models.Dtos
{
    public class FileDownloadDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        //private string Type;

       // public void setType(string _ContentType) {
       //     if (ContentType == "pdf")
       //     {
       //         Type = "Aplication";
       //     }
       //     else { Type = ""; }
       //}


    }
}
