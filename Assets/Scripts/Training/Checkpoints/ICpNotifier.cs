using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpNotifier
{
    //private List<ICpListener> _listeners;

    public void NotifyListeners();
}
