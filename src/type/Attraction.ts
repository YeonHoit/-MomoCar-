import type {AttractionPhoto, AttractionSupport} from '../type'

export type Attraction = {
  Index_num: number
  Name: string
  Kind: string
  Photo: AttractionPhoto[]
  Subtitle: string
  Lat: number
  Long: number
  Address: string
  PhoneNumber: string
  OpenHours: string
  Menu: string
  Support: AttractionSupport[]
}
