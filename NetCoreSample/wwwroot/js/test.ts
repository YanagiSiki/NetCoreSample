(function (w) {
$('#french_calendar')
  .calendar({
    text: {
      days: ['D', 'L', 'M', 'M', 'J', 'V', 'S'],
      monthsShort: ['Jan', 'Fev', 'Mar', 'Avr', 'Mai', 'Juin', 'Juil', 'Aou', 'Sep', 'Oct', 'Nov', 'Dec'],
      today: 'Aujourd\'hui',
      now: 'Maintenant',
      am: 'AM',
      pm: 'PM'
    },

  });
}(window));