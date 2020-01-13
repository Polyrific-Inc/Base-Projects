import { Injectable } from '@angular/core';
import { DarkThemeConfig, WhiteThemeConfig } from '../data/config/theme-config';

@Injectable({
  providedIn: 'root'
})
export class CustomizerService {

  constructor() {
    let data = WhiteThemeConfig.data;
    var layoutVersion = localStorage.getItem("layoutVersion");
    if(layoutVersion == 'dark-only'){
        data = DarkThemeConfig.data;
    }

    document.body.className = data.color.mix_layout;
    document.body.setAttribute("main-theme-layout", data.settings.layout_type);
    document.getElementsByTagName('html')[0].setAttribute('dir', data.settings.layout_type);
    var color = data.color.color;
    var layoutVersion = data.color.layout_version;
    if (color) {
      this.createStyle(color);
      if (layoutVersion)
        document.body.className = layoutVersion;
    }
  }

  // Create style sheet append in head
  createStyle(color) {
    var head = document.head;
    var link = document.createElement("link");
    link.type = "text/css";
    link.href = window.location.origin + "assets/css/" + color + ".css";
    head.appendChild(link);
  }

}
