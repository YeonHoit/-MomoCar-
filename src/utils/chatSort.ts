import type {ChatRoom} from '../type'
import moment from 'moment-with-locales-es6'

export const chatSort = (a: ChatRoom, b: ChatRoom): number => {
  let date1 = moment(a.LastChat.SendDate).toDate().getTime()
  let date2 = moment(b.LastChat.SendDate).toDate().getTime()
  if (date1 > date2) {
    return -1
  } else if (a.LastChat.Index_num < b.LastChat.Index_num) {
    return 1
  }
  return 0
}
