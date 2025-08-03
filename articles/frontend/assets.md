# Frontend Varlıkları Dokümantasyonu

Bu belge, BT Varlık Yönetimi uygulamasındaki statik varlıklara genel bir bakış sağlar.

## Dizin Yapısı

```
wwwroot/
├── css/
│   ├── site.css               # Ana uygulama stilleri
│   └── bootstrap.min.css      # Bootstrap framework stilleri
├── js/
│   ├── site.js               # Ana uygulama scriptleri
│   ├── jquery.min.js         # jQuery kütüphanesi
│   ├── bootstrap.bundle.js   # Bootstrap JavaScript
│   └── qrcode.min.js        # QR kod üretme kütüphanesi
├── lib/                      # Üçüncü parti kütüphaneler
│   ├── bootstrap/
│   ├── jquery/
│   ├── jquery-validation/
│   └── jquery-validation-unobtrusive/
└── images/                   # Uygulama görselleri ve ikonları
    ├── logo.png
    └── favicon.ico
```

## CSS Files

### site.css
Main application stylesheet containing custom styles for:
- Layout and navigation
- Forms and inputs
- Tables and lists
- Cards and panels
- Mobile responsiveness
- Custom components

### Bootstrap Theme Customizations
Bootstrap variables and overrides for:
- Primary colors: #007bff
- Secondary colors: #6c757d
- Success: #28a745
- Danger: #dc3545
- Font families: Segoe UI, system-ui
- Border radius: 0.25rem
- Spacing units: 1rem base

## JavaScript Files

### site.js
Main application JavaScript containing:
- AJAX form submissions
- Dynamic table sorting
- Search functionality
- QR code generation
- Date picker initialization
- Form validation
- Modal dialogs
- Toast notifications

### Third-party Libraries
- jQuery v3.6.0
- Bootstrap v5.1.0
- QR Code Generator v1.4.4
- jQuery Validation
- jQuery Unobtrusive Validation

## Images

### Application Images
- Logo: 200x50px PNG with transparency
- Favicon: 32x32px ICO format
- Default profile picture: 200x200px JPG

## Asset Management

### Bundling and Minification
Production builds automatically:
1. Minify CSS files
2. Minify JavaScript files
3. Combine common libraries
4. Generate source maps

### Cache Busting
- Files include version hash in production
- Cache headers set to 1 year for static files
- Version query string added automatically

### CDN Support
Production environment uses CDN for:
- Bootstrap
- jQuery
- Font Awesome

### Mobile Optimization
- Responsive images with srcset
- CSS media queries
- Touch-friendly interactions
- Mobile-first design approach

## Development Guidelines

### Adding New Styles
1. Add new styles to appropriate section in site.css
2. Follow BEM naming convention
3. Include mobile-first media queries
4. Document significant changes

### Adding New Scripts
1. Add new functions to appropriate module in site.js
2. Use jQuery noConflict mode
3. Document function purpose and parameters
4. Add error handling

### Image Guidelines
1. Optimize all images for web
2. Include responsive versions
3. Use appropriate format (PNG for transparency, JPG for photos)
4. Keep file sizes under 200KB

### Performance Considerations
1. Lazy load images
2. Defer non-critical JavaScript
3. Minimize HTTP requests
4. Use CSS sprites for icons
5. Enable compression

## Browser Support
- Chrome (last 2 versions)
- Firefox (last 2 versions)
- Safari (last 2 versions)
- Edge (last 2 versions)
- iOS Safari
- Chrome for Android
