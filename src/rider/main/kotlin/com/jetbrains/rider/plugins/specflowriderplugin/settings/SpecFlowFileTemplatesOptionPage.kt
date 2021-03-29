package com.jetbrains.rider.settings.templates

import com.intellij.openapi.options.Configurable
import com.jetbrains.rider.settings.simple.SimpleOptionsPage

class SpecFlowFileTemplatesOptionPage: SimpleOptionsPage("SpecFlow", "RiderFeatureFileTemplatesSettings"), Configurable.NoScroll {
    override fun getId(): String {
        return "RiderFeatureFileTemplatesSettings"
    }
}