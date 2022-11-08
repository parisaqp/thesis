using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalAdmin.infrastracture
{
    public static class FileUploadFormHelper
    {
        public static MvcHtmlString FileUploadForm(this HtmlHelper htmlHelper, string ControllerAction)
        {
            string formTemplate =
                   @"<form action='{0}' method='post' 
                      enctype='multipart/form-data' class = 'dropzone' 
 id = 'dropzoneJsForm'
 style = 'background-color:#00BFFF' >
                                        <div class='fallback'>
                                            <input name='file' type='file' multiple />
                                            <input type='submit' value='Upload' />
                                        </div>
                                    </form>";
            return new MvcHtmlString(string.Format(formTemplate, ControllerAction)); ;
        }
    }
}