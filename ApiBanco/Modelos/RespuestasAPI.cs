﻿using System.Net;

namespace ApiBanco.Modelos
{
    public class RespuestasAPI
    {
        public RespuestasAPI()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}

