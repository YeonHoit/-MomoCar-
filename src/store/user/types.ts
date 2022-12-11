import type {Action} from 'redux'

export type User = {
  Index_ID: number
  ID: string
  Login: string
  Nickname: string
  UserCar: string
  Photo: string
  Email: string
  Phone: string
  NoticeAlert: boolean
  CommentAlert: boolean
  LikeAlert: boolean
  TagAlert: boolean
  ChattingAlert: boolean
  UserCarType: string
}

export type State = {
  Index_ID: number
  ID: string
  Login: string
  Nickname: string
  UserCar: string
  Photo: string
  Email: string
  Phone: string
  NoticeAlert: boolean
  CommentAlert: boolean
  LikeAlert: boolean
  TagAlert: boolean
  ChattingAlert: boolean
  UserCarType: string
}

export type SetUserAction = Action<'@user/setUser'> & State
export type DistroyUserAction = Action<'@user/distroyUser'>

export type Actions = SetUserAction | DistroyUserAction
