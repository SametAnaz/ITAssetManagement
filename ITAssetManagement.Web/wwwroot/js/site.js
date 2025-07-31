// Tema değiştirme fonksiyonları
const themeToggle = document.getElementById('themeToggle');
const themeIcon = document.getElementById('themeIcon');
const themeText = document.getElementById('themeText');

// Tema tercihini localStorage'dan al
const currentTheme = localStorage.getItem('theme') || 'light';
document.documentElement.setAttribute('data-theme', currentTheme);
updateThemeUI(currentTheme);

// Tema değiştirme butonu tıklama olayı
themeToggle.addEventListener('click', () => {
    const currentTheme = document.documentElement.getAttribute('data-theme');
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
    
    document.documentElement.setAttribute('data-theme', newTheme);
    localStorage.setItem('theme', newTheme);
    updateThemeUI(newTheme);
});

// Tema UI güncellemesi
function updateThemeUI(theme) {
    if (theme === 'dark') {
        themeIcon.classList.remove('fa-sun');
        themeIcon.classList.add('fa-moon');
        themeText.textContent = 'Aydınlık Tema';
    } else {
        themeIcon.classList.remove('fa-moon');
        themeIcon.classList.add('fa-sun');
        themeText.textContent = 'Karanlık Tema';
    }
}

// Toastr tema uyumu için
if (typeof toastr !== 'undefined') {
    const toastrThemes = {
        'light': 'toastr-light',
        'dark': 'toastr-dark'
    };

    // Toastr temasını güncelle
    function updateToastrTheme(theme) {
        Object.values(toastrThemes).forEach(className => {
            document.body.classList.remove(className);
        });
        document.body.classList.add(toastrThemes[theme]);
    }

    // İlk yüklemede Toastr temasını ayarla
    updateToastrTheme(currentTheme);

    // Tema değiştiğinde Toastr temasını güncelle
    themeToggle.addEventListener('click', () => {
        const newTheme = document.documentElement.getAttribute('data-theme');
        updateToastrTheme(newTheme);
    });
}