using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web;

namespace BenFarms.MVC.Models
{
    public class Utils
    {
        private static readonly MyApplicationDbContext storeDB = new MyApplicationDbContext();

        internal delegate void FillDataTextPage(HttpRequest httpRequest, string form, IPagePreview preview);

        internal delegate void FillDataImagePage(string fileName, string file, IPagePreview preview);

        internal static async Task<KeyValuePair<bool, string>> PrepareDataPage(string page, string dir, string pageName, IPagePreview preview, FileType? fileType, FillDataTextPage fillDataTextPage, FillDataImagePage fillDataImagePage)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    if (!file.Contains("client_imageEncabezado"))
                    {
                        var postedFile = httpRequest.Files[file];
                        var valid = IsValidImage(postedFile, fileType);

                        if (valid.Value == "Ok")
                        {
                            if (postedFile != null)
                            {
                                var fil = $"{Path.GetFileNameWithoutExtension(postedFile.FileName)}_{DateTime.Now.Ticks}{postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'))}";
                                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(dir), fil);
                                //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(dir), postedFile.FileName);
                                if (!postedFile.FileName.StartsWith("/"))
                                {
                                    postedFile.SaveAs(fileSavePath);
                                    //var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                                    //if (ext == ".pdf")
                                    //{
                                    //    var fileName = SaveToPdf.Save(HttpContext.Current.Server.MapPath(dir), fil);
                                    //    fileName = Path.GetFileName(fileName);
                                    //    if (fileName != null)
                                    //    {
                                    //        fillDataImagePage($"{dir}/{fileName}", file, preview);
                                    //    }
                                    //}
                                    //else
                                    //{
                                        fillDataImagePage($"{dir}/{fil}", file, preview);
                                    //}
                                }
                            }
                        }
                        else
                        {
                            return valid;
                        }
                    }
                }

                foreach (var form in httpRequest.Form.AllKeys)
                {
                    var inicio_texto_c = "client_TextEncabezado";
                    var inicio_color_c = "client_ColorTextEncabezado";
                    var inicio_texto_s = "server_TextEncabezado";
                    var inicio_color_s = "server_ColorTextEncabezado";
                    var serverHeaderLink = "server_HeaderLink";
                    var clientHeaderLink = "client_HeaderLink";
                    if (!form.Contains(inicio_texto_c) && !form.Contains(inicio_color_c) && !form.Contains(inicio_texto_s) && !form.Contains(inicio_color_s) &&
                        !form.Contains("idsCliente") && !form.Contains("idsServidor") && !form.Contains(serverHeaderLink) &&
                        !form.Contains(clientHeaderLink))
                    {
                        fillDataTextPage(httpRequest, form, preview);
                    }
                }

                KeyValuePair<bool, string> validFilesHead;
                var heads = FillDataImage("",httpRequest, dir, fileType, page, out validFilesHead);

                if (validFilesHead.Value == "Ok" && heads != null)
                {
                    preview.Encabezado = heads;
                }

                if(page == "BillingPage")
                {
                    KeyValuePair<bool, string> validFilesHead1;
                    var heads1 = FillDataImage("Lab", httpRequest, dir, fileType, "LabSection", out validFilesHead1);

                    if (validFilesHead1.Value == "Ok" && heads1 != null)
                    {
                        var preview1 = preview as BillingPagePreview;
                        if (preview1 != null) preview1.ImagesLab = heads1;
                    }
                }


                //if (page == "FosePage")
                //{
                //    KeyValuePair<bool, string> validFilesHead1;
                //    var heads1 = FillDataImage("Fose", httpRequest, dir, fileType, "FosePromocion", out validFilesHead1);

                //    if (validFilesHead1.Value == "Ok" && heads1 != null)
                //    {
                //        var preview1 = preview as FosePagePreview;
                //        if (preview1 != null) preview1.Promocions = heads1;
                //    }
                //}

                //if (page == "BlogPage")
                //{
                //    KeyValuePair<bool, string> validFilesHead1;
                //    var heads1 = FillDataImage("Blog", httpRequest, dir, fileType, "BlogNews", out validFilesHead1);

                //    if (validFilesHead1.Value == "Ok" && heads1 != null)
                //    {
                //        var preview1 = preview as BlogPagePreview;
                //        if (preview1 != null) preview1.NewsPagesBlogs = heads1;
                //    }
                //}

                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, preview);
                stream.Close();

                var pagePreview = await storeDB.PagePreviews.FindAsync(pageName);
                if (pagePreview != null)
                {
                    storeDB.PagePreviews.Remove(pagePreview);
                    await storeDB.SaveChangesAsync();
                }

                storeDB.PagePreviews.Add(new PagePreview { PageName = pageName, PageValue = stream.GetBuffer() });
                await storeDB.SaveChangesAsync();

                return new KeyValuePair<bool, string>(true, "Los datos fueron enviados correctamente al servidor.");
            }
            catch (Exception)
            {
                return new KeyValuePair<bool, string>(false, "Ocurrió un error al enviar los datos al servidor");
            }
        }

        private static List<HeadImage> FillDataImage(string clave, HttpRequest httpRequest, string dir, FileType? fileType, string page, out KeyValuePair<bool, string> validHeads)
        {
            List<HeadImage> listHeads = new List<HeadImage>();

            var formValue = httpRequest.Form[$"idsCliente{clave}"];
            formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
            if (formValue != null)
            {
                var splitIds = formValue.Split(',');
                var count = splitIds.Length - 1;
                var inicio_imagen = $"client_imageEncabezado{clave}_";
                var inicio_texto = $"client_TextEncabezado{clave}_";
                var inicio_color = $"client_ColorTextEncabezado{clave}_";
                var clientHeaderLink = $"client_HeaderLink{clave}_"; 

                for (var i = 0; i < count; i++)
                {
                    var clave_img = inicio_imagen + splitIds[i];
                    var clave_texto = inicio_texto + splitIds[i];
                    var clave_color = inicio_color + splitIds[i];
                    var linkKey = clientHeaderLink + splitIds[i];

                    var postedFile = httpRequest.Files[clave_img];
                    if (postedFile != null)
                    {
                        var head = new HeadImage
                        {
                            HeadImageActive = true,
                        };
                        var valid = IsValidImage(postedFile, fileType);

                        if (valid.Value == "Ok")
                        {
                            var fil = $"{Path.GetFileNameWithoutExtension(postedFile.FileName)}_{DateTime.Now.Ticks}{postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'))}";
                            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(dir), fil);

                            //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(dir), postedFile.FileName);
                            if (!postedFile.FileName.StartsWith("/"))
                            {
                                postedFile.SaveAs(fileSavePath);
                                //var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                                //if (ext == ".pdf")
                                //{
                                //    var fileName = SaveToPdf.Save(HttpContext.Current.Server.MapPath(dir), fil);
                                //    fileName = Path.GetFileName(fileName);
                                //    if (fileName != null)
                                //    {
                                //        head.HeadImageImage = $"{dir}/{fileName}";
                                //    }
                                //}
                                //else
                                //{
                                    head.HeadImageImage = $"{dir}/{fil}";
                                //}

                                head.HeadImageText = httpRequest.Form[clave_texto];
                                head.HeadImageColorText = httpRequest.Form[clave_color];
                                head.HeadImagePageName = page;
                                head.HeaderLink = httpRequest.Form[linkKey]; 
                            }
                        }
                        else
                        {
                            validHeads = valid;
                            return null;
                        }

                        listHeads.Add(head);
                    }
                }
            }

            var formValue_s = httpRequest.Form[$"idsServidor{clave}"];
            formValue_s = string.IsNullOrEmpty(formValue_s) ? null : formValue_s;
            if (formValue_s != null)
            {
                var inicio_texto_s = $"server_TextEncabezado{clave}_";
                var inicio_color_s = $"server_ColorTextEncabezado{clave}_";
                var serverHeaderLink = $"server_HeaderLink{clave}_";

                var splitIds_s = formValue_s.Split(',');
                var count_s = splitIds_s.Length - 1;

                for (var i = 0; i < count_s; i++)
                {
                    var id = int.Parse(splitIds_s[i]);
                    var clave_texto = inicio_texto_s + id;
                    var clave_color = inicio_color_s + id;
                    var linkKey = serverHeaderLink + id;

                    var image = storeDB.ImageSections.FirstOrDefault(x => x.ImageSectionId == id && x.ImageSectionPageName == page);
                    if (image != null)
                    {
                        var head = new HeadImage
                        {
                            HeadImageActive = true,
                            HeadImagePageId = image.ImageSectionPageId,
                            HeadImageImage = image.ImageSectionImage,
                            HeadImageText = httpRequest.Form[clave_texto],
                            HeadImageColorText = httpRequest.Form[clave_color],
                            HeadImagePageName = page,
                            HeaderLink = httpRequest.Form[linkKey]
                    };
                        listHeads.Add(head);
                    }
                    else
                    {
                        validHeads = new KeyValuePair<bool, string>(false, "No Existe Imagen");
                        return null;
                    }
                }
            }

            validHeads = new KeyValuePair<bool, string>(true, "Ok");
            return listHeads;
        }

        internal static KeyValuePair<bool, string> IsValidImage(HttpPostedFile postedFile, FileType? fileType)
        {
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                const int MaxContentLength = 1024 * 1024 * 10; //Size = 1 MB  

                var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                var extension = ext.ToLower();
                var AllowedFileExtensions = GetListExtensions(fileType);
                if (!AllowedFileExtensions.Contains(extension))
                {
                    return new KeyValuePair<bool, string>(false, "Solo se admiten archivos de tipo: " + GetToStringListExtensions(fileType));
                }
                if (postedFile.ContentLength > MaxContentLength)
                {
                    return new KeyValuePair<bool, string>(false, "Solo se admiten imágenes de tamaño <= 10 MB.");
                }
                return new KeyValuePair<bool, string>(true, "Ok");
            }
            return new KeyValuePair<bool, string>(false, "Se requiere seleccionar una imagen.");
        }

        private static List<string> GetListExtensions(FileType? fileType)
        {
            if (fileType != null)
            {
                switch (fileType)
                {
                    case FileType.All:
                        return new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".xls", ".xlsx", ".pdf", ".doc", ".docx" };
                    case FileType.Image:
                        return new List<string> { ".jpg", ".jpeg",  ".png", ".gif" };
                    case FileType.ImagePdf:
                        return new List<string> { ".jpg", ".png", ".gif", ".pdf" };
                    case FileType.Pdf:
                        return new List<string> { ".pdf" };
                    case FileType.Excel:
                        return new List<string> { ".xls", ".xlsx" };
                    case FileType.Word:
                        return new List<string> { ".doc", ".docx" };
                }
            }
            return null;
        }

        private static string GetToStringListExtensions(FileType? fileType)
        {
            if (fileType != null)
            {
                switch (fileType)
                {
                    case FileType.All:
                        return ".jpg, .png, .gif, .xls, .xlsx, .pdf, .doc, .docx";
                    case FileType.Image:
                        return ".jpg, .png, .gif";
                    case FileType.ImagePdf:
                        return ".jpg, .png, .gif, .pdf";
                    case FileType.Pdf:
                        return ".pdf";
                    case FileType.Excel:
                        return ".xls, .xlsx";
                    case FileType.Word:
                        return ".doc, .docx";
                }
            }
            return "";
        }

        public static IList<ImageSection> ConvertToImageSectionList(List<HeadImage> encabezadoInicio)
        {
            return encabezadoInicio.Select(ConvertToImageSection).ToList();
        }

        public static ImageSection ConvertToImageSection(HeadImage x)
        {
            ImageSection image = new ImageSection
            {
                ImageSectionActive = true,
                ImageSectionColorText = x.HeadImageColorText,
                ImageSectionImage = x.HeadImageImage,
                ImageSectionPageName = x.HeadImagePageName,
                ImageSectionText = x.HeadImageText,
                Link = x.HeaderLink
            };
            image.ImageSectionPageId = x.HeadImagePageName == "FosePromocion" || x.HeadImagePageName == "BlogNews" ? x.HeadImagePageId : 0;

            return image;
        }
    }

    public class ImageUpload
    {
        public string FileName { get; set; }

        public bool IsUploaded { get; set; }
    }

    //public static class SaveToPdf
    //{
    //    public static string Save(string dir, string fileName)
    //    {
    //        try
    //        {
    //            var fil = $"{Path.GetFileNameWithoutExtension(fileName)}";
    //            PdfDocument documemt = new PdfDocument();
    //            var path = Path.Combine(dir, fileName);
    //            documemt.LoadFromFile(path);
    //            //var count = documemt.Pages.Count;
    //            //for (int i = 0; i < count; i++)
    //            //{
    //            Image image = documemt.SaveAsImage(0, PdfImageType.Bitmap, 400, 400);
    //            var pdf = Path.Combine(dir, $"{fil}.jpg");
    //            //var pdf = Path.Combine(HttpContext.Current.Server.MapPath(dir), $"{fileName}_{i}.jpg");
    //            image.Save(pdf);
    //            //}
    //            documemt.Close();
    //            return pdf;
    //        }
    //        catch (Exception)
    //        {
    //            return null;
    //        }
    //    }
    //}

    public enum FileType
    {
        All,
        Image,
        ImagePdf,
        Pdf,
        Excel,
        Word
    }
}