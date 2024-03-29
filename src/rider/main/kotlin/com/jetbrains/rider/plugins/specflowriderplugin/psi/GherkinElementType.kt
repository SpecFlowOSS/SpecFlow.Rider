// Copyright 2000-2021 JetBrains s.r.o. Use of this source code is governed by the Apache 2.0 license that can be found in the LICENSE file.
package com.jetbrains.rider.plugins.specflowriderplugin.psi

import com.intellij.psi.tree.IElementType
import org.jetbrains.annotations.NonNls

class GherkinElementType(debugName: @NonNls String) : IElementType(debugName, GherkinLanguage)
