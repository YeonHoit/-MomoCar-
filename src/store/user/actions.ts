import type * as T from './types'

export const setUserAction = (user: T.User): T.SetUserAction => ({
  type: '@user/setUser',
  Index_ID: user.Index_ID,
  ID: user.ID,
  Login: user.Login,
  Nickname: user.Nickname,
  UserCar: user.UserCar,
  Photo: user.Photo,
  Email: user.Email,
  Phone: user.Phone,
  NoticeAlert: user.NoticeAlert,
  CommentAlert: user.CommentAlert,
  LikeAlert: user.LikeAlert,
  TagAlert: user.TagAlert,
  ChattingAlert: user.ChattingAlert,
  UserCarType: user.UserCarType
})

export const distroyUserAction = (): T.DistroyUserAction => ({
  type: '@user/distroyUser'
})
