using System;
using System.Collections;
using System.Collections.Generic;

namespace WotDossier.Applications.Logic.Export
{
    public interface IExportProviderBase
    {
        string Export(IList list, List<Type> exportInterfaces);
    }
}
