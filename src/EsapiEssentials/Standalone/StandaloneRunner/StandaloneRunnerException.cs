﻿using System;

namespace EsapiEssentials.Standalone
{
    public class StandaloneRunnerException : Exception
    {
        public StandaloneRunnerException(string message)
            : base(message) { }

        public StandaloneRunnerException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}