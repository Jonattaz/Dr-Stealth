/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

namespace Mz.Parameters {
    public partial class Parameter {
        public delegate void ValueChangedEventHandler(object parameter, object valuePrevious);
        public delegate void AttachedEventHandler(object parameter);
        public delegate void DetachedEventHandler(object parameter, object parentPrevious);
        public delegate void CollectionAttachedEventHandler(object set);
        public delegate void CollectionDetachedEventHandler(object set, object parentPrevious);
    }
}
