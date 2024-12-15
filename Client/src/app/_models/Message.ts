export interface Message {
    id: number
    sourcePhotoUrl: string
    sourceUsername: string
    sourceUserId: number
    targetPhotoUrl: string
    targetUsername: string
    targetUserId: number
    content: string
    readDate?: Date
    sentDate: Date
  }