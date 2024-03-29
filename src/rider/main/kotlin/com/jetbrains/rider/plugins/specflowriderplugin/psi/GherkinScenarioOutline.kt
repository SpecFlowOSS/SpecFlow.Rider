// Copyright 2000-2021 JetBrains s.r.o. Use of this source code is governed by the Apache 2.0 license that can be found in the LICENSE file.
package com.jetbrains.rider.plugins.specflowriderplugin.psi

interface GherkinScenarioOutline : GherkinStepsHolder {
    val examplesBlocks: List<GherkinExamplesBlock>
    val outlineTableMap: Map<String, String>?
}
