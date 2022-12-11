import {
  KakaoOAuthToken,
  KakaoProfile,
  getProfile as getKakaoProfile,
  login,
  logout
} from '@react-native-seoul/kakao-login'

export const signInWithKakao = async (): Promise<void> => {
  const token: KakaoOAuthToken = await login()
}

export const signOutWithKakao = async (): Promise<void> => {
  const message = await logout()
}

export const getProfile = async (): Promise<KakaoProfile> => {
  const profile: KakaoProfile = await getKakaoProfile()

  return new Promise<KakaoProfile>((resolve, reject) => {
    resolve(profile)
  })
}
