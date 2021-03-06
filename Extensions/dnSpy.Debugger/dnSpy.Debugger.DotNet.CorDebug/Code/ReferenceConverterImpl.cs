﻿/*
    Copyright (C) 2014-2018 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using dnSpy.Contracts.Debugger.DotNet.Code;
using dnSpy.Contracts.Documents;

namespace dnSpy.Debugger.DotNet.CorDebug.Code {
	[ExportReferenceConverter]
	sealed class ReferenceConverterImpl : ReferenceConverter {
		public override void Convert(ref object reference) {
			switch (reference) {
			case DbgDotNetNativeCodeLocationImpl nativeLoc:
				switch (nativeLoc.ILOffsetMapping) {
				case DbgILOffsetMapping.Exact:
				case DbgILOffsetMapping.Approximate:
					reference = new DotNetMethodBodyReference(nativeLoc.Module, nativeLoc.Token, nativeLoc.Offset);
					break;

				case DbgILOffsetMapping.Prolog:
					reference = new DotNetMethodBodyReference(nativeLoc.Module, nativeLoc.Token, DotNetMethodBodyReference.PROLOG);
					break;

				case DbgILOffsetMapping.Epilog:
					reference = new DotNetMethodBodyReference(nativeLoc.Module, nativeLoc.Token, DotNetMethodBodyReference.EPILOG);
					break;

				case DbgILOffsetMapping.Unknown:
				case DbgILOffsetMapping.NoInfo:
				case DbgILOffsetMapping.UnmappedAddress:
				default:
					// The IL offset isn't known so use a method reference
					reference = new DotNetTokenReference(nativeLoc.Module, nativeLoc.Token);
					break;
				}
				break;
			}
		}
	}
}
