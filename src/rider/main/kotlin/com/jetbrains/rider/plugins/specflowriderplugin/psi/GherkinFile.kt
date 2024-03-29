// Copyright 2000-2021 JetBrains s.r.o. Use of this source code is governed by the Apache 2.0 license that can be found in the LICENSE file.
package com.jetbrains.rider.plugins.specflowriderplugin.psi

import com.intellij.psi.PsiFile

interface GherkinFile : PsiFile {
    val stepKeywords: List<String?>
    val localeLanguage: String
    val features: Array<GherkinFeature>
}
