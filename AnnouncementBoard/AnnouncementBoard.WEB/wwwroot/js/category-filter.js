const subCategories = {
    "Побутова техніка": ["Холодильники", "Пральні машини", "Бойлери", "Печі", "Витяжки", "Мікрохвильові печі"],
    "Комп'ютерна техніка": ["ПК", "Ноутбуки", "Монітори", "Принтери", "Сканери"],
    "Смартфони": ["Android смартфони", "iOS/Apple смартфони"],
    "Інше": ["Одяг", "Взуття", "Аксесуари", "Спортивне обладнання", "Іграшки"]
};

function onCategoryChange() {
    const categorySelect = document.getElementById('categorySelect');
    const subCategorySelect = document.getElementById('subCategorySelect');
    const selectedCategory = categorySelect.value;

    subCategorySelect.innerHTML = '<option value="">-- Виберіть підкатегорію --</option>';

    if (selectedCategory && subCategories[selectedCategory]) {
        subCategories[selectedCategory].forEach(subCat => {
            const option = document.createElement('option');
            option.value = subCat;
            option.text = subCat;
            subCategorySelect.appendChild(option);
        });
    }
}
document.addEventListener("DOMContentLoaded", onCategoryChange);