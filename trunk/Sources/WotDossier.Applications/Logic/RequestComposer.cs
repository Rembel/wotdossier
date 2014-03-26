using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WotDossier.Applications.Logic
{
    public class RequestComposer
    {
        private const string REQ_BOUNDARY = "---------------------------{0}";

        private const string FORM_DATA_TITLE = "Content-Disposition: form-data; name=\"Replay[title]\"\r\n\r\n{0}";
        private const string FORM_DATA_DESCRIPTION = "Content-Disposition: form-data; name=\"Replay[description]\"\r\n\r\n{0}";
        private const string FORM_DATA_IS_SECRET = "Content-Disposition: form-data; name=\"Replay[isSecret]\"\r\n\r\n{0}";
        private const string FORM_DATA_FILENAME = "Content-Disposition: form-data; name=\"Replay[file_name]\"; filename=\"{0}\"\r\n" + CONTENT_TYPE_OCTET_STREAM;
        private const string FORM_DATA_YT0 = "Content-Disposition: form-data; name=\"yt0\"\r\n\r\nUpload";
        private const string CONTENT_TYPE_OCTET_STREAM = "Content-Type: application/octet-stream\r\n\r\n";

        readonly List<byte> _bytes = new List<byte>();
        private readonly string _boundary = string.Format(REQ_BOUNDARY, DateTime.Now.Ticks.ToString("x"));
        private readonly string _separator;
        private readonly string _requestEnd;

        private StringBuilder _builder = new StringBuilder();

        public string Boundary
        {
            get { return _boundary; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public RequestComposer()
        {
            _separator = "\r\n--" + _boundary + "\r\n";
            _requestEnd = "\r\n--" + _boundary + "--\r\n";
        }

        public RequestComposer Description(string description)
        {
            Append(Encoding.UTF8.GetBytes(_separator));
            _builder.Append(_separator);
            string descriptionData = string.Format(FORM_DATA_DESCRIPTION, description);
            _builder.Append(descriptionData);
            Append(Encoding.UTF8.GetBytes(descriptionData));
            return this;
        }

        public RequestComposer Secret(string isSecret)
        {
            Append(Encoding.UTF8.GetBytes(_separator));
            Append(Encoding.UTF8.GetBytes(string.Format(FORM_DATA_IS_SECRET, isSecret)));
            return this;
        }

        public RequestComposer File(FileInfo file)
        {
            string fileName = string.Format(FORM_DATA_FILENAME, file.Name);
            byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(file.FullName);

            Append(Encoding.UTF8.GetBytes(_separator));
            Append(fileNameBytes);
            Append(fileBytes);
            return this;
        }

        public RequestComposer Title(string title)
        {
            Append(Encoding.UTF8.GetBytes(_separator));
            Append(Encoding.UTF8.GetBytes(string.Format(FORM_DATA_TITLE, title)));
            return this;
        }

        public RequestComposer End()
        {
            Append(Encoding.UTF8.GetBytes(_separator));
            Append(Encoding.UTF8.GetBytes(FORM_DATA_YT0));
            Append(Encoding.UTF8.GetBytes(_requestEnd));
            return this;
        }

        private void Append(byte[] bytes)
        {
            _bytes.AddRange(bytes);
        }

        public byte[] GetRequestBytes()
        {
            return _bytes.ToArray();
        }
    }
}
