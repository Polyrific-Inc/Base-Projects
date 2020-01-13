export class WhiteThemeConfig {
    static data = {
        settings: {
            layout_type: 'ltr',
            sidebar: {
                type: 'default',
                body_type: 'default'
            },
            sidebar_setting: 'default-sidebar',
            sidebar_backround: 'dark-sidebar'
        },
        color: {
            layout_version: 'light',
            color: 'color-1',
            primary_color: '#4466f2',
            secondary_color: '#1ea6ec',
            mix_layout: 'default'
        },
        router_animation: 'fadeIn'
    }
}

export class DarkThemeConfig {
    static data = {
        settings: {
            layout_type: 'ltr',
            sidebar: {
                type: 'default',
                body_type: 'default'
            },
            sidebar_setting: 'default-sidebar',
            sidebar_backround: 'dark-sidebar'
        },
        color: {
            layout_version: 'dark-only',
            color: 'color-1',
            primary_color: '#000',
            secondary_color: '#ff9f40',
            mix_layout: 'default'
        },
        router_animation: 'fadeIn'
    }
}

