﻿using UnityEngine;

        if (ghostlyShadow) {
            shadow.GetComponent<SpriteRenderer>().color = PowerupInfo.GHOST_COLOR;
        else {
            shadow.GetComponent<SpriteRenderer>().color = normalColor;
        }
    }