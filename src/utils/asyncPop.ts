import type {CommunityPost, Block} from '../type'
import {post} from '../server'

export const asyncPop = (
  communityList: CommunityPost[],
  blockList: Block[],
  UserID: string
) =>
  new Promise<CommunityPost[]>((resolve, reject) => {
    for (let j = 0; j < blockList.length; j++) {
      if (blockList[j].Target === UserID) {
        communityList.pop()
        break
      }
    }
    resolve(communityList)
  })
