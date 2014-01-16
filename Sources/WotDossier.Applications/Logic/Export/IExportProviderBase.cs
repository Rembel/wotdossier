using System;
using System.Collections;
using System.Collections.Generic;

namespace WotDossier.Applications.Logic.Export
{
    public interface IExportProviderBase
    {
        void Export(IList list, List<Type> exportInterfaces);
    }
}
