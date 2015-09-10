using System;
using System.Collections;
using System.Collections.Generic;

namespace WotDossier.Applications.Logic.Export
{
    public abstract class ExportProviderBase : IExportProviderBase
    {
        public abstract string Export(IList list, List<Type> exportInterfaces);
    }
}