using System;
using NMagnify.Model;

namespace NMagnify
{
    public interface IActiveProfileProvider
    {
        event EventHandler<ProfileEventArgs> ActiveProfileChanged;
        Profile ActiveProfile { get; }
    }
}