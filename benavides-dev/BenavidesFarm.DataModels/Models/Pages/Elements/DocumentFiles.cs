using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de archivo de Documento
    /// </summary>
    public class DocumentFiles
    {
        public int DocumentFilesId { get; set; }

        public int DocumentTypeId { get; set; }

        [Display(Name = "Nombre a mostrar del archivo")]
        public string NameDescriptiveFile { get; set; }

        [Display(Name = "Archivo")]
        public string AddressFile { get; set; }

        public string GetFileName => !string.IsNullOrEmpty(AddressFile) ? AddressFile.Replace("~/ProviderDocuments/", "") : "";

        public DocumentType DocumentType { get; set; }
    }
}