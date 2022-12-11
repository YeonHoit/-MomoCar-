import moment from 'moment-with-locales-es6'

export const momentCal = (date_string: string) => {
  const target_date_moment = moment(date_string)
  const target_date = target_date_moment.toDate().getTime()
  const current_date = new Date().getTime()
  const secondDiff = (current_date - target_date) / 1000
  const minuteDiff = (current_date - target_date) / (1000 * 60)
  const hourDiff = (current_date - target_date) / (1000 * 3600)
  const dateDiff = (current_date - target_date) / (1000 * 3600 * 24)
  const monthDiff = (current_date - target_date) / (1000 * 3600 * 24 * 30)
  const yearDiff = (current_date - target_date) / (1000 * 3600 * 24 * 365)
  // if (dateDiff >= 1) {
  //   return target_date_moment.format('MM[월] DD[일]')
  // } else {
  //   return target_date_moment.format('a h:mm')
  // }
  if (yearDiff >= 1) {
    return Math.floor(yearDiff) + '년전'
  } else if (monthDiff >= 1) {
    if (Math.floor(monthDiff) == 12) {
      return '11개월전'
    }
    return Math.floor(monthDiff) + '개월전'
  } else if (dateDiff >= 1) {
    return Math.floor(dateDiff) + '일전'
  } else if (hourDiff >= 1) {
    return Math.floor(hourDiff) + '시간전'
  } else if (minuteDiff >= 1) {
    return Math.floor(minuteDiff) + '분전'
  } else {
    return Math.floor(secondDiff) + '초전'
  }
}
