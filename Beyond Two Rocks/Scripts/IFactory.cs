using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IFactory<T, P>
{
    T Create(P obj);
}
