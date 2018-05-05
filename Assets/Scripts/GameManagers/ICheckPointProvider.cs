using System.Collections.Generic;

namespace GameManagers
{
    public interface ICheckPointProvider
    {
        List<CheckPoint> CheckPoints { get; }
    }
}