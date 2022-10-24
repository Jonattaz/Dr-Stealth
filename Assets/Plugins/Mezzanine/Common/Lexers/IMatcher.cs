/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System.Collections.Generic;

namespace Mz.Lexers {
    public interface IMatcher {
        Token MatchLineStart(string inputString);
        IEnumerable<Token> MatchAll(string inputString);
    }
}
