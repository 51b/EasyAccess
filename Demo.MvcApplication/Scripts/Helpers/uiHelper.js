﻿var uiHelper = {
    icon: {
        success: "<img src='/Content/images/icon_success.png' />",
        fail: "<img src='/Content/images/icon_fail.png' />",
        edit: "<img src='/Content/images/icon_edit.png' />",
        remove: "<img src='/Content/images/icon_remove.png' />",
        save: "<img src='/Content/images/icon_save.png' />",
        undo: "<img src='/Content/images/icon_undo.png' />"
    },
    color: {
        font: {
            info: "#3A87AD",
            error: "#C54A48",
            warning: "#C0988D",
            success: "#468847"
        },
        background: {
            info: "#D9EDF7",
            error: "#F2DEDE",
            warning: "#FCF8E3",
            success: "#DFF0D8"
        }
    }
};



$(function() {
    global.uiHelper = uiHelper;
});