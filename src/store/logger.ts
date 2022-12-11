import {Action, Dispatch} from 'redux'

export function logger<S = any>({getState}: {getState: () => S}) {
  return (next: Dispatch) => (action: Action) => {
    const returnValue = next(action)
    return returnValue
  }
}
