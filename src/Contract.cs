//===--- Contract.cs ------------------------------------------------------===//
//
// Copyright (c) 2015 Joe Duffy. All rights reserved.
//
// This file is distributed under the MIT License. See LICENSE.md for details.
//
//===----------------------------------------------------------------------===//

namespace System
{
    static class Contract
    {
        public static void Abandon()
        {
            Environment.FailFast("A program error has occurred.");
        }

        public static void Assert(bool condition)
        {
            if (!condition) {
                Abandon();
            }
        }

        public static void Requires(bool condition)
        {
            if (!condition) {
                throw new ArgumentException();
            }
        }

        public static void RequiresInRange(int start, int length)
        {
            if (!(start >= 0 && start < length)) {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static void RequiresInInclusiveRange(int start, int length)
        {
            if (!(start >= 0 && start <= length)) {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static void RequiresInInclusiveRange(int start, int end, int length)
        {
            if (!(start >= 0 && start <= end && end >= 0 && end <= length)) {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}

