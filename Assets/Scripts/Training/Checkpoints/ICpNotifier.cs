using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpNotifier
{
    // Obsolete. Leaving it in for now
    
    //private List<ICpListener> _listeners;

    public void NotifyListeners();
}
