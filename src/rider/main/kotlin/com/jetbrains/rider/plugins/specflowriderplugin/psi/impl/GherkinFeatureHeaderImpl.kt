// Copyright 2000-2021 JetBrains s.r.o. Use of this source code is governed by the Apache 2.0 license that can be found in the LICENSE file.
package com.jetbrains.rider.plugins.specflowriderplugin.psi.impl

import com.intellij.lang.ASTNode

class GherkinFeatureHeaderImpl(node: ASTNode) : GherkinPsiElementBase(node) {

    override fun toString(): String {
        return "GherkinFeatureHeader"
    }
}
