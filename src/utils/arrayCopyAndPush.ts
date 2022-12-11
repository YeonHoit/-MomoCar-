import type {Chat} from '../type'

export const arrayCopyAndPush = (
  arr: Chat[] | undefined,
  target: Chat
): Chat[] => {
  return arr ? [...arr, target] : [target]
}
