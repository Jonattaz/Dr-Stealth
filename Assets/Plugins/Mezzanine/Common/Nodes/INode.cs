/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.Collections.Generic;

namespace Mz.Nodes {
    public interface INode<TNode, out TRoot> 
        where TNode : class, INode<TNode, TRoot>
        where TRoot : class, TNode
    {
        //===== Properties
        object Key { get; }
        string Path { get; }
        bool IsRoot { get; }
        
        TRoot Root { get; }
        TNode Parent { get; }
        TNode NextSibling { get; }
        TNode PreviousSibling { get; }
        TNode FirstChild { get; }
        TNode LastChild { get; }
        
        List<TNode> Children { get; }
        
        //===== Events
        event NodeAttachedEventHandler Attached;
        event NodeDetachedEventHandler Detached;
        
        //===== Methods
        TNode RemoveByKey(object key);
        TNode GetByKey(object key);
        TNode Get(string path, char pathSegmentDelimiter = '/');

        TNode GetFirstSibling();
        List<TNode> Unravel(List<TNode> list = null);

        void Walk(Func<TNode, bool> action);
    }
}