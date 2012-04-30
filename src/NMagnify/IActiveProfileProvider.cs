using System;
using NMagnify.Model;

namespace NMagnify
{
    public interface IActiveProfileProvider
    {
        Profile ActiveProfile { get; }
        event EventHandler<ProfileEventArgs> ActiveProfileChanged;
    }
}