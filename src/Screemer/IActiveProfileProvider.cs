using System;
using Screemer.Model;

namespace Screemer
{
    public interface IActiveProfileProvider
    {
        event EventHandler<ProfileEventArgs> ActiveProfileChanged;
        Profile ActiveProfile { get; }
    }
}